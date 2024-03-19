using SendGrid.Helpers.Mail;
using SendGrid;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/SendEmail", async (
    string toName, 
    string toEmail,
    string fromName,
    string fromEmail,
    string subject, 
    string plainTextContent,
    string htmlContent) =>
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        var client = new SendGridClient(apiKey);
        var to = new EmailAddress(toEmail, toName);
        var from = new EmailAddress(fromEmail, fromName);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

        return response;
    }
)
.WithName("SendEmail")
.WithOpenApi();

app.Run();
