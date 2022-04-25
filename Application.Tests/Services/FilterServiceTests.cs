﻿using Application.DTOs.InputModels;
using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Services
{
    internal class FilterServiceTests
    {
        private FilterService _sut;

        private IQueryable<Advertisement> _advertisements;

        [SetUp]
        public void Setup()
        {
            _sut = new FilterService();

            _advertisements = new List<Advertisement>
            {
                new Advertisement()
                {
                    Building = new Building()
                    {
                        Address = new Address
                        {
                            City = new City()
                            {
                                Id = 1
                            }
                        }
                    },
                    Id = 1,
                    Price = 1500
                },
                new Advertisement()
                {
                    Id = 2,
                    Price = 1000,
                    Building = new Building()
                    {
                        Address = new Address
                        {
                            City = new City()
                            {
                                Id = 2
                            }
                        }
                    },
                },
                new Advertisement()
                {
                    Id = 3,
                    Price = 2000,
                    Building = new Building()
                    {
                        Address = new Address
                        {
                            City = new City()
                            {
                                Id = 3
                            }
                        }
                    },
                },
                new Advertisement()
                {
                    Id = 4,
                    Price = 3000,
                    Building = new Building()
                    {
                        Address = new Address
                        {
                            City = new City()
                            {
                                Id = 4
                            }
                        }
                    },
                },
            }.AsQueryable();
        }

        [Test]
        public void FilterDown_MaxAndMinPriceSet_FilterCorrectly()
        {
            var request = new FilterRequest()
            {
                MinPrice = 1000,
                MaxPrice = 2000,
            };

            var result = _sut.FilterDown(_advertisements, request);
            
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(3);
        }

        [Test]
        public void FilterDown_CityIdAndPricesSet_FilterCorrectly()
        {
            var request = new FilterRequest()
            {
                CityId = 1,
                MinPrice = 1000,
                MaxPrice = 2000,
            };

            var result = _sut.FilterDown(_advertisements, request);

            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(1);
            result.Data.Single().Id.Should().Be(1);
        }


        [Test]
        public void FilterDown_FiltersDontMatch_ReturnsEmpty()
        {
            var request = new FilterRequest()
            {
                CityId = 1,
                MinPrice = 4000,
                MaxPrice = 8000,
            };

            var result = _sut.FilterDown(_advertisements, request);

            result.Success.Should().BeTrue();
            result.Data.Should().BeEmpty();
        }
    }
}
