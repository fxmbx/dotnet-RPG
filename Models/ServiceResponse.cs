using System;
using System.Collections.Generic;
using dotnet_RPG.Dtos.Character.User;

namespace dotnet_RPG.Models
{
    public class ServiceResponse <T>
    {
        public T Data {get; set;}

        public bool Success {get; set;} = true;

        public string Meassgae {get;set;} = null;


    }
}