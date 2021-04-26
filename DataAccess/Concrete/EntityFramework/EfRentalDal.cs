using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRentalDal : EfRepositoryBase<Rental, RentACarContext>, IRentalDal
    {
        public List<RentalDetailDto> GetRentalDetails(Expression<Func<Rental, bool>> filter = null)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from rentals in filter is null ? context.Rentals : context.Rentals.Where(filter)
                             join cars in context.Cars
                             on rentals.CarId equals cars.Id
                             join customers in context.Customers
                             on rentals.CustomerId equals customers.Id
                             join users in context.Users
                             on customers.UserId equals users.Id
                             join brands in context.Brands
                             on cars.BrandId equals brands.Id
                             join colors in context.Colors
                             on cars.ColorId equals colors.Id
                             select new RentalDetailDto
                             {
                                 Id = rentals.Id,
                                 CarName=cars.CarName,
                                 CustomerName=users.FirstName,
                                 CompanyName=customers.CompanyName,
                                 BrandName=brands.BrandName,
                                 ColorName=colors.ColorName,
                                 ModelYear=cars.ModelYear,
                                 RentDate=rentals.RentDate,
                                 ReturnDate=rentals.ReturnDate
                             };

                return result.ToList();
            }
        }
    }
}
