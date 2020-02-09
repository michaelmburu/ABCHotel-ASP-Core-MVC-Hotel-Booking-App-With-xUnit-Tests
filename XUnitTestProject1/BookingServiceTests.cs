using ABCHotel.Data; 
using ABCHotel.Repositories;
using ABCHotel.Services;
using Moq;
using System;
using Xunit;

namespace ABCHotel.Tests
{
    public class BookingServiceTests
    {
        private Mock<IRoomsRepository> roomRepo;
        private Mock<ICouponRepository> couponRepo;
        public BookingServiceTests()
        {
            //Before each test
            roomRepo = new Mock<IRoomsRepository>();
            couponRepo = new Mock<ICouponRepository>();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room());
        }

 
        public BookingService Subject()
        {
            return new BookingService(roomRepo.Object, couponRepo.Object);
        }

        [Fact]
        public void IsBookingValid_Non_Smoker_Valid()
        {
            var service = Subject();
            var isValid = service.IsBookingValid(1, new Booking() { IsSmoking = false });
            Assert.True(isValid);
        }

        [Fact]
        public void IsBookingValid_Smoker_Valid()
        {
            var service = Subject();
            var isValid = service.IsBookingValid(1, new Booking() { IsSmoking = true });
            Assert.False(isValid);
        }

        //Test scenario where pets are not allowed in a room and guest has brought pets

        [Fact]
        public void IsBookingValid_PetsNotAllowed_Invalid()
        {
            var service =  Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room() { ArePetsAllowed = false });
            var isValid = service.IsBookingValid(1, new Booking { HasPets = true });

            Assert.False(isValid);

        }

        //Test Scenario where pets are allowed in a room and guest and guest has brought pets
        [Fact]
        public void IsBookingValid_PetsAllowed_IsValid()
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { ArePetsAllowed = true });
            var isValid = service.IsBookingValid(1, new Booking { HasPets = true });
            Assert.True(isValid);
        }

        //Test Scenario where pets are  allowed and user has not brought pets
        [Fact]
        public void IsBookingValid_NoPetsAllowed_Isvalid()
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { ArePetsAllowed = true });
            var isValid = service.IsBookingValid(1, new Booking { HasPets = false });
            Assert.True(isValid);
        }

        //Test Scenario where pets are not allowed and a user has brought pets 
        [Fact]
        public void IsBookingValid_NoPetsAllowed_IsValid()
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { ArePetsAllowed = false });
            var isValid = service.IsBookingValid(1, new Booking { HasPets = false});

            Assert.True(isValid);
        }

        //Testing scenario where we test the four above tests with a theory
        [Theory]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        public void IsBookingValid_Pets(bool arePetsAllowed, bool hasPets, bool result)
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { ArePetsAllowed = arePetsAllowed });
            var isValid = service.IsBookingValid(1, new Booking { HasPets = hasPets });

            Assert.Equal(isValid, result);
        }

        //Test scenario where room capcity is less than guest booking capacity
        [Fact]
        public void IsBookingValid_GuestsLessThanCapacity_Valid()
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { Capacity = 4 });
            var isValid = service.IsBookingValid(1, new Booking { NumberOfGuests = 1 });

            Assert.True(isValid);
        }

        //Test Scenario where room capacity is more than guest booking capacity
        [Fact]
        public void IsBookingValid_GuestsMaxThanCapacity_Valid()
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { Capacity = 2 });
            var isValid = service.IsBookingValid(1, new Booking { NumberOfGuests = 10 });
            Assert.False(isValid);
        }

        [Fact]
        public void CalculateBookingPrice_CalculatesCorrectly()
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room {Rate = 250});
            var price = service.CalculateBookingPrice(new Booking { Id =1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(2) });
            Assert.Equal(500, price);
        }

        [Fact]
        public void CalculateBookingPrice_CalculatesCorrectly_WithEmptyCouponCode()
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { Rate = 250 });
            var price = service.CalculateBookingPrice(new Booking { Id = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(2), CouponCode ="" });
            Assert.Equal(500, price);
        }

        [Fact]
        public void CalculateBookingPrice_DiscountsCoupon()
        {
            var service = Subject();
            roomRepo.Setup(r => r.GetRoom(1)).Returns(new Room { Rate = 250 });
            couponRepo.Setup(c => c.GetCoupon("10OFF")).Returns(new Coupon() { PercentageDiscount = 10 });

            var price = service.CalculateBookingPrice(new Booking { Id = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(2), CouponCode = "10OFF" });

            Assert.Equal(450, price);
        }


    }
}
