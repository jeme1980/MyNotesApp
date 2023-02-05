using MyNotesApp.Data;
using System.Diagnostics;

namespace MyNotesApp
{
    public static class STInitializer
    {
        public static void seed(IApplicationBuilder applicationBuilder) 
        { 
            ApplicationDbContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                if (!context.Students.Any())
                {
                    context.Students.Add(new Models.Student() { StudentFirstName = "ali", StudentLasttName = "ali" });
                    context.SaveChanges();  
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
                throw;
            }
        }
    }
}
