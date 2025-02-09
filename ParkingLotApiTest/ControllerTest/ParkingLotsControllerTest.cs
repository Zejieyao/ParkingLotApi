﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EFCoreRelationshipsPracticeTest;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ParkingLotApi.Dtos;

namespace ParkingLotApiTest.ControllerTest
{
    public class ParkingLotsControllerTest : TestBase
    {
        public ParkingLotsControllerTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parking_lot_success()
        {
            var client = this.GetClient();

            var parkingLotDto = this.CreateParkingLotDto("ParkingLot-1", 10);

            StringContent content = this.SerializeParkingDto(parkingLotDto);
            await client.PostAsync("/ParkingLot", content);

            var allParkingLotsResponseMessage = await client.GetAsync("/ParkingLot?page=1");
            var allParkingLotsString = await allParkingLotsResponseMessage.Content.ReadAsStringAsync();
            var allParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(allParkingLotsString);

            Assert.Single(allParkingLots);
        }

        [Fact]
        public async Task Should_get_parking_lot_by_id_success()
        {
            var client = this.GetClient();

            var parkingLotDto = this.CreateParkingLotDto("ParkingLot-1", 0);

            StringContent content = this.SerializeParkingDto(parkingLotDto);
            var returnedIdMessage = await client.PostAsync("/ParkingLot", content);

            var parkingLotResponse = await client.GetAsync(returnedIdMessage.Headers.Location);
            var body = await parkingLotResponse.Content.ReadAsStringAsync();
            var returnParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(body);

            Assert.Equal("ParkingLot-1", returnParkingLot.Name);
        }

        [Fact]
        public async Task Should_delete_parking_lot_by_id_success()
        {
            var client = this.GetClient();
            var parkingLotDto = this.CreateParkingLotDto("ParkingLot-1", 0);

            StringContent content = this.SerializeParkingDto(parkingLotDto);
            var returnedIdMessage = await client.PostAsync("/ParkingLot", content);

            await client.DeleteAsync(returnedIdMessage.Headers.Location);

            var allParkingLotsResponseMessage = await client.GetAsync("/ParkingLot?page=1");
            var allParkingLotsString = await allParkingLotsResponseMessage.Content.ReadAsStringAsync();
            var allParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(allParkingLotsString);

            Assert.Empty(allParkingLots);
        }

        [Fact]
        public async Task Should_update_parking_lot_capacity_success()
        {
            var client = this.GetClient();
            var parkingLotDto = this.CreateParkingLotDto("ParkingLot-1", 0);

            StringContent content = this.SerializeParkingDto(parkingLotDto);
            var returnedIdMessage = await client.PostAsync("/ParkingLot", content);

            parkingLotDto.Capacity = 5567;

            StringContent capcityContent = this.SerializeParkingDto(parkingLotDto);

            var updatedParkingLotMessage = await client.PutAsync(returnedIdMessage.Headers.Location, capcityContent);
            var parkingLotString = await updatedParkingLotMessage.Content.ReadAsStringAsync();
            var updatedParkingLot = JsonConvert.DeserializeObject<ParkingLotDto>(parkingLotString);

            Assert.Equal(5567, updatedParkingLot.Capacity);
        }

        private ParkingLotDto CreateParkingLotDto(string name, int capacity)
        {
            return new ParkingLotDto()
            {
                Name = name,
                Location = "Beijing",
                Capacity = capacity,
            };
        }

        private StringContent SerializeParkingDto(ParkingLotDto parkingLotDto)
        {
            return new StringContent(JsonConvert.SerializeObject(parkingLotDto), Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}
