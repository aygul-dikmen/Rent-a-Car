using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Entities.Dtos;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfRepositoryBase<Car, RentACarContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from cars in filter is null ? context.Cars : context.Cars.Where(filter)
                             join brands in context.Brands
                             on cars.BrandId equals brands.Id
                             join colors in context.Colors
                             on cars.ColorId equals colors.Id
                             select new CarDetailDto
                             {
                                 Id = cars.Id,
                                 CarName = cars.CarName,
                                 BrandName = brands.BrandName,
                                 ColorName = colors.ColorName,
                                 DailyPrice = cars.DailyPrice,
                                 ModelYear = cars.ModelYear
                             };

                return result.ToList();
            }
        }
    }
}
