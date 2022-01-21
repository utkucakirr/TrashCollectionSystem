﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.DataModel;
using Data.Uow;
using Entity;
using Entity.DTOs;
using Microsoft.Extensions.Logging;

namespace TrashCollectionSystem.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VehicleController> _logger;
        private readonly IMapper _mapper;

        public VehicleController(ILogger<VehicleController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Getting all vehicles from database.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listOfVehicles = await _unitOfWork.VehicleRepository.GetAll();
            if (listOfVehicles is null)
            {
                return Ok("List is empty");
            }

            return Ok(listOfVehicles);
        }

        //Getting the vehicle whose id is equal to the parameter value.
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetById(id);
            if (vehicle is null)
            {
                return NotFound();
            }

            //Converting datamodel to entity using AutoMapper.
            var entity = _mapper.Map<Vehicle, VehicleEntity>(vehicle);
            entity.ServerDate = DateTime.UtcNow;

            return Ok(entity);
        }

        //Creating a vehicle. For this method, VehicleViewModel is used. By this way, the user don't have to enter id value. This value will be generated by database.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleViewModel entity)
        {
            //Converting VehicleViewModel to Vehicle model for adding to the database using AutoMapper.
            var addedEntity = _mapper.Map<VehicleViewModel, Vehicle>(entity);
            var response = await _unitOfWork.VehicleRepository.Add(addedEntity);
            _unitOfWork.Complete();
            if (response)
            {
                return Ok("Vehicle added.");
            }

            return Ok("Failed.");
        }

        //Updating a vehicle.
        //Bu metotta datamodel yerine entity kullanmak ve convert işlemi ile aradaki farkı gösterebilmek için-
        //- güncelleme işleminde aracın plakası değiştirilemez kuralı kabul edilmiştir.
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] VehicleUpdateModel entity)
        {
            //Converting VehicleUpdateModel to Vehicle using AutoMapper
            var updatedEntity = _mapper.Map<VehicleUpdateModel, Vehicle>(entity);
            updatedEntity.VehiclePlate = _unitOfWork.VehicleRepository.GetById(entity.id).Result.VehiclePlate;
            var response = await _unitOfWork.VehicleRepository.Update(updatedEntity);
            _unitOfWork.Complete();
            if (response)
            {
                return Ok("Updated.");
            }

            return Ok("Failed.");
        }

        //Deleting a vehicle.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = await _unitOfWork.VehicleRepository.Delete(id);
            _unitOfWork.Complete();
            if (response)
            {
                return Ok("Deleted succesfully.");
            }

            return Ok("Failed.");
        }

        //Getting all containers of the vehicle whose id is entered as parameter.
        [HttpGet("Containers/{id}")]
        public async Task<IActionResult> GetContainers(long id)
        {
            var listOfContainers = await _unitOfWork.ContainerRepository.GetByVehicleId(id);
            _unitOfWork.Complete();
            if (listOfContainers is null)
            {
                return Ok("There is no container for that vehicle.");
            }

            return Ok(listOfContainers);
        }

        //In this method, the containers of the vehicle whose id is entered as parameter, will be divided into n groups.
        [HttpGet("ContainersClustered")]
        public async Task<IActionResult> GetContainerGroups(long id, int n)
        {
            var response = await _unitOfWork.ContainerRepository.GroupByVehicleId(id, n);
            return Ok(response);
        }
    }
}