using ConsoleTestApp.Contracts.Dto;
using ConsoleTestApp.EfCore;
using ConsoleTestApp.EfCore.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace ConsoleTestApp
{
    internal class Program
    {

        public static ApplicationDbContext _context = new ApplicationDbContext();

        public static MemoryStream _memoryStream = new MemoryStream();

        public static HttpClient _httpClient = new HttpClient();

        public static int _iterator = 0;


        static async Task Main(string[] args)
        {
            if (_context.ApplicationUsers.Any())
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }
            for (_iterator = 0; _iterator < 1000; _iterator++)
            {
                await InsertUserAsync().ConfigureAwait(true);
            }
        }


        public static async Task<(UserDto, byte[])> GetUserDtoAsync()
        {           
            var response = await _httpClient.GetAsync("https://randomuser.me/api/?inc=name,dob,picture");

            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());

            var userDto = responseDto.Results.First();

            var photoResponse = await _httpClient.GetAsync(userDto.Picture.Thumbnail);

            var photoStream = await photoResponse.Content.ReadAsStreamAsync();

            photoStream.CopyTo(_memoryStream);

            var photoContent = _memoryStream.ToArray();

            return (userDto, photoContent);
        }

        public static async Task InsertUserAsync()
        {
            try
            {
                (var userDto, var photoContent) = await GetUserDtoAsync();

                using (var transactionScope = new TransactionScope())
                {
                    var user = new ApplicationUser()
                    {
                        BirthDate = userDto.Dob.Date,
                        FullName = userDto.Name.Title + " " + userDto.Name.First + " " + userDto.Name.Last,
                        PhotoUri = userDto.Picture.Thumbnail
                    };

                    _context.ApplicationUsers.Add(user);

                    _context.SaveChanges();

                    var photo = new ApplicationUserPhoto()
                    {
                        ApplicationUserId = user.Id,
                        Photo = photoContent
                    };

                    _context.Photos.Add(photo);

                   _context.SaveChanges();

                    // при работе с транзакцией приходится использовать SaveChanges() вместо SaveChangesAsync(),
                    // потому что поток, который ее открыл, должен ее и закончить, а при неблокирующем ожидании
                    // await, поток возвращается в пул и контекст подхватывает другой поток, даже при включенном
                    // ConfigureAwait(true), хотя там и так по умолчанию этот параметр выставлен, в общем хз,
                    // в отпуске сражаться с этим багом посчитал оверхедом, поэтому оставил два синхронных SaveChanges();
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + $"\n Выполнение программы столкнулось с ошибкой на {_iterator} - операции");
            }

        }
    }
}




