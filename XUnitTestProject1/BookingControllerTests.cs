using ABCHotel.Controllers;
using ABCHotel.Data;
using ABCHotel.Models;
using ABCHotel.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ABCHotel.Tests
{
    public class BookingControllerTests
    {
        private Mock<IRoomService> roomService;
        private Mock<IBookingService> bookingService;

        public BookingControllerTests()
        {
            roomService = new Mock<IRoomService>();
            bookingService = new Mock<IBookingService>();
        }

        public BookingController Subject()
        {
            return new BookingController(roomService.Object, bookingService.Object);
        }

        [Fact]
        public void IndexPost_CalculatePrice_WithCheckInDate()
        {
            var service = Subject();
            service.Index(new BookingViewModel { CheckInDate = DateTime.MinValue });
            bookingService.Verify(s => s.CalculateBookingPrice(It.Is((Booking b) => b.CheckInDate == DateTime.MinValue)));
        }

        [Fact]
        public void IndexPost_CalculatePrice_WithCheckOutDate()
        {
            var service = Subject();
            service.Index(new BookingViewModel { CheckOutDate = DateTime.MaxValue });
            bookingService.Verify(s => s.CalculateBookingPrice(It.Is((Booking b) => b.CheckOutDate == DateTime.MaxValue)));
        }

        [Fact]
        public void IndexPost_Calculate_WithRoomId()
        {
            var service = Subject();
            service.Index(new BookingViewModel { RoomId = 404 });
            bookingService.Verify(s => s.CalculateBookingPrice(It.Is((Booking b) => b.RoomId == 404)));

        }
        [Fact]
        public void IndexPost_Calculate_WithCouponCode()
        {
            var service = Subject();
            service.Index(new BookingViewModel { CouponCode = "FREEMONEY" });
            bookingService.Verify(s => s.CalculateBookingPrice(It.Is((Booking b) => b.CouponCode == "FREEMONEY")));

        }

    }
}
