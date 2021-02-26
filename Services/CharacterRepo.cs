using System.Collections.Generic;
using dotnet_RPG.Models;
using System.Linq;
using System.Threading.Tasks;
using dotnet_RPG.Dtos.Character;
using AutoMapper;
using System;
using dotnet_RPG.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections;

namespace dotnet_RPG.Services
{
    public class CharacterRepo : ICharacter
    {
        private readonly IMapper mapper;
        private readonly DataContext  dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CharacterRepo(IMapper _mapper, DataContext _dbContext, IHttpContextAccessor _httpContextAccessor)
        {
            mapper = _mapper;
            dbContext = _dbContext;
            httpContextAccessor = _httpContextAccessor;
        }

        private int GetUserId() => int.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public  async Task<ServiceResponse<IEnumerable<GetCharacterDto>>> AddCharacter(AddCharacterDto charac)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetCharacterDto>>();
            var charact = (mapper.Map<Character>(charac));
            charact.users = await dbContext.users.FirstOrDefaultAsync(x=>x.Id == GetUserId());

            await dbContext.characters.AddAsync(charact);
            await dbContext.SaveChangesAsync();
            serviceResponse.Data = (dbContext.characters.Where(x=>x.users.Id == GetUserId()).Select(c =>mapper.Map<GetCharacterDto>(c)));
            return serviceResponse ;
        }

        public async Task<ServiceResponse<IEnumerable<GetCharacterDto>>> GetAllCharacters( )
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetCharacterDto>>();
            var dbCharact = await dbContext.characters.Where(x=>x.users.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharact.Select(c => mapper.Map<GetCharacterDto>(c));
             return serviceResponse;
        }
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbcharacter = await dbContext.characters.FirstOrDefaultAsync(c =>c.Id == id && c.users.Id == GetUserId());
            serviceResponse.Data = mapper.Map<GetCharacterDto>(dbcharacter);

             return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateChararcterDto update)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
              var charact =  await dbContext.characters.FirstOrDefaultAsync(c =>c.Id == update.Id);

            if(charact.users.Id == GetUserId())
            {
                charact.Id = update.Id;
                charact.Name = update.Name;
                charact.Intelligence = update.Intelligence;
                charact.Strength = update.Strength;
                charact.HitPoints = update.HitPoints;
                charact.Defense = update.Defense;

                dbContext.characters.Update(charact);
                await dbContext.SaveChangesAsync();

                serviceResponse.Data = mapper.Map<GetCharacterDto>(charact);

            }else
            {
                serviceResponse.Success=false;
                serviceResponse.Meassgae="Character not found";
            }
            }
            catch(Exception e){
                serviceResponse.Success = false;
                serviceResponse.Meassgae = e.Message;
            }
                return serviceResponse;

        }

        public async Task<ServiceResponse<IEnumerable<GetCharacterDto>>> DeleteCharacter(int id)
        {
           var serviceResponse = new ServiceResponse<IEnumerable <GetCharacterDto>> ();
           try
           {
             var charac = await dbContext.characters.FirstOrDefaultAsync(c =>c.Id == id && c.users.Id == GetUserId());
             if(charac != null){
             dbContext.characters.Remove(charac);
             await dbContext.SaveChangesAsync();
             serviceResponse.Data = (dbContext.characters.Where(c =>c.users.Id == GetUserId()).Select(c =>mapper.Map<GetCharacterDto>(c)));
             }
                 serviceResponse.Success = false;
                 serviceResponse.Meassgae = "Two things 1)No be you get Character, 2) Character no de";
           }catch (Exception ex){
               serviceResponse.Success = false;
               serviceResponse.Meassgae = ex.Message;
           }
            return serviceResponse;
        }


    }
}