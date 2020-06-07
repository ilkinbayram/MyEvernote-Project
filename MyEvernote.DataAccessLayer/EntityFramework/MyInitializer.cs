using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.EntitiesLayer;
using CryptoHelper;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<MyEvernoteDBContext>
    {
        protected override void Seed(MyEvernoteDBContext context)
        {
            // Admin creating
            User admin = new User
            {
                Name = "Ilkin",
                Surname = "Bayramov",
                Username = "ilkinbayram",
                Password = "demoxxx123",
                Email = "ilkasdasd@gmail.com",
                IsAdmin = true,
                IsConfirmed = true,
                IsDeleted = false,
                IsBanned = false,
                Token = Guid.NewGuid(),
                Gender = true,
                ImageRoad = "admin_boy_profilePhoto.png",
                ConfirmCode = Guid.NewGuid(),
                CreatedOn = DateTime.Now.AddDays(-13),
                ModifiedOn = DateTime.Now.AddDays(-7),
                ModifiedUsername = "ilkinbayram"
            };
            context.Users.Add(admin);

            // Fake Users creating
            for (int f = 0; f < 12; f++)
            {
                string fkn = FakeData.NameData.GetFirstName();
                string fksn = FakeData.NameData.GetSurname();
                User simpleUser = new User
                {
                    Name = fkn,
                    Surname = fksn,
                    Username = fkn+FakeData.NumberData.GetNumber(100,500).ToString(),
                    Password = "demoxxx123",
                    Email = fkn + FakeData.NumberData.GetNumber(100, 500).ToString() + "@gmail.com",
                    IsAdmin = false,
                    IsConfirmed = true,
                    IsDeleted = false,
                    IsBanned = false,
                    ConfirmCode = Guid.NewGuid(),
                    Token = Guid.NewGuid(),
                    Gender = (f%2==1 ? true:false),
                    ImageRoad = (f%2==1 ? "boy_profilePhoto.png" : "girl_profilePhoto.png"),
                    CreatedOn = DateTime.Now.AddDays(-10),
                    ModifiedOn = DateTime.Now.AddDays(-3),
                    ModifiedUsername = "ilkinbayram"
                };
                context.Users.Add(simpleUser);
            }
            
            context.SaveChanges();

            List<User> usersRuntime = context.Users.ToList();

            // Category Loop Start
            for (int i = 0; i < 10; i++)
            {
                Category ctg = new Category
                {
                    CategoryName = FakeData.NameData.GetCompanyName(),
                    Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(2, 3)),
                    IsDeleted = false,
                    CreatedOn = DateTime.Now.AddDays(-(FakeData.NumberData.GetNumber(5, 7))),
                    ModifiedOn = DateTime.Now.AddDays(-(FakeData.NumberData.GetNumber(1, 4))),
                    ModifiedUsername = "ilkinbayram"
                };
                
                // Notes Loop Start
                for (int k = 0; k < FakeData.NumberData.GetNumber(3,7); k++)
                {
                    int ind = FakeData.NumberData.GetNumber(0, 12);
                    Note note = new Note
                    {
                        IsDeleted = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 13),
                        IsDraft = false,
                        CreatedOn = ctg.CreatedOn.AddMinutes(FakeData.NumberData.GetNumber(15, 45)),
                        ModifiedOn = ctg.ModifiedOn.AddMinutes(FakeData.NumberData.GetNumber(15, 45)),
                        User = usersRuntime[ind],
                        ModifiedUsername = usersRuntime[ind].Username,
                        NoteTitle = FakeData.PlaceData.GetStreetName(),
                        Text = FakeData.TextData.GetSentences(4)
                    };
                    
                    ctg.Notes.Add(note);
                    

                    // Comments Loop Start
                    for (int t = 0; t < FakeData.NumberData.GetNumber(4,11); t++)
                    {
                        int indc = FakeData.NumberData.GetNumber(0, 12);
                        Comment cmt = new Comment
                        {
                            CreatedOn = note.CreatedOn.AddMinutes(FakeData.NumberData.GetNumber(15, 45)),
                            ModifiedOn = note.ModifiedOn.AddMinutes(FakeData.NumberData.GetNumber(15, 45)),
                            ModifiedUsername = usersRuntime[indc].Username,
                            User = usersRuntime[indc],
                            Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1,4))
                        };
                        note.Comments.Add(cmt);
                        context.Comments.Add(cmt);
                    } // Comments Loop Finish


                    // Likes Loop Start
                    for (int s = 0; s < note.LikeCount; s++)
                    {
                        Liked liked = new Liked
                        {
                            User = usersRuntime[FakeData.NumberData.GetNumber(0,12)]
                        };
                        note.Likes.Add(liked);
                        context.Likes.Add(liked);
                    } // Likes Loop Finish



                    context.Notes.Add(note);
                } // Notes Loop Finish


                context.Categories.Add(ctg);
            } // Category Loop Finish

            context.SaveChanges();
        }
    }
}
