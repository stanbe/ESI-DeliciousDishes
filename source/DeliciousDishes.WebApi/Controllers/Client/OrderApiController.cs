﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DeliciousDishes.DataAccess.Context;
using DeliciousDishes.DataAccess.Entities;
using DeliciousDishes.WebApi.Filter;
using DeliciousDishes.WebApi.Models.Client;

namespace DeliciousDishes.WebApi.Controllers.Client
{
    public class OrderApiController : ApiController
    {
        [Route("client/order")]
        [HttpPost]
        [EnsureHasContentFilter]
        [ValidateModelFilter]
        public IHttpActionResult NewOrder([FromBody] MenuOrderDto order)
        {
            try
            {
                using (var context = new DeliciousDishesDbContext())
                {
                    var menuOrder = new MenuOrder
                    {
                        OrderUser = order.OrderUserId,
                        DailyOfferId = order.DailyOfferId,
                        RecipientUser = order.RecipientUserId,
                        Remarks = order.Remarks
                    };
                    context.MenuOrders.Add(menuOrder);
                    context.SaveChanges();

                    return this.Created("/order/" + menuOrder.Id, order);
                }
            }
            catch (Exception)
            {
                // Todo:Exception handling
                throw;
            }
        }

        [Route("client/order/{menuOrderId}")]
        [HttpGet]
        public IHttpActionResult ShowOrder(long menuOrderId)
        {
            try
            {
                using (var context = new DeliciousDishesDbContext())
                {
                    var menuOrder = context.MenuOrders.Find(menuOrderId);
                    var menuOrderDto = new MenuOrderDto
                    {
                        MenuOrderId = menuOrder.Id,
                        OrderUserId = menuOrder.OrderUser,
                        DailyOfferId = menuOrder.DailyOfferId,
                        RecipientUserId = menuOrder.RecipientUser,
                    };
                    return this.Ok(menuOrderDto);
                }
            }
            catch (Exception)
            {
                // Todo:Exception handling
                throw;
            }
        }

        [Route("client/order")]
        [HttpPut]
        [EnsureHasContentFilter]
        [ValidateModelFilter]
        public IHttpActionResult UpdateOrder([FromBody] MenuOrderDto order)
        {
            return this.Ok(order);
        }

        [Route("client/order")]
        [HttpDelete]
        public IHttpActionResult CancelOrder(long orderId)
        {
            // TODO: Cancel the Order
            return this.Ok();
        }

        [Route("client/order/")]
        [HttpGet]
        public IHttpActionResult ShowList(string user, DateTime date)
        {
            var orderUserId = user;

            var random = new Random(DateTime.UtcNow.Millisecond);
            
            var orders = new List<MenuOrderDto>(new[]
            {
                new MenuOrderDto()
                {
                    DailyOfferId = 12345,
                    MenuOrderId = random.Next(0, 1000),
                    OrderUserId = orderUserId,
                    RecipientUserId = "other",
                }, 
                new MenuOrderDto()
                {
                    DailyOfferId = 12345,
                    MenuOrderId = random.Next(0, 1000),
                    OrderUserId = orderUserId,
                    RecipientUserId = orderUserId,
                }, 
            });

            return this.Ok(orders);
        }
    }
}
