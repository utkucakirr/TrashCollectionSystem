using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOs;
using Data.DataModel;
using Entity;

namespace Core
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<VehicleViewModel, Vehicle>();

            CreateMap<Vehicle, VehicleEntity>();
            CreateMap<VehicleUpdateModel, Vehicle>();

            CreateMap<Container, ContainerViewModel>();
            CreateMap<ContainerViewModel, Container>();
            CreateMap<ContainerUpdateModel, Container>();
        }
    }
}
