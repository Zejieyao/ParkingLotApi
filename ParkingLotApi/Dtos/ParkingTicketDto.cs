﻿using System;
using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos
{
    public class ParkingTicketDto
    {
        public ParkingTicketDto() {}

        public ParkingTicketDto(ParkingTicketEntity parkingTicketEntity)
        {
           ParkingLotName = parkingTicketEntity.ParkingLotName;
           PlateNumber = parkingTicketEntity.PlateNumber;
           CreateTime = parkingTicketEntity.CreateTime;
           CloseTime = parkingTicketEntity.CloseTime;
           OrderStatus = parkingTicketEntity.OrderStatus;
        }

        public string ParkingLotName { get; set; }

        public string PlateNumber { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? CloseTime { get; set; }

        public bool OrderStatus { get; set; }

        public ParkingTicketEntity ToEntity()
        {
            return new ParkingTicketEntity()
            {
                ParkingLotName = this.ParkingLotName,
                PlateNumber = this.PlateNumber,
                CreateTime = this.CreateTime,
                CloseTime = this.CloseTime != null ? this.CloseTime : null,
                OrderStatus = this.OrderStatus,
            };
        }
    }
}
