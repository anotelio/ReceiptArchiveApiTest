using ReceiptArchiveApi.Core.ApiFilters;
using ReceiptArchiveApi.Core.Middlewares;
using ReceiptArchiveApi.Core.ProgramExtensions;
using ReceiptArchiveApi.Core.ProgramExtensions.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureKafka(builder.Configuration);

builder.Services.AddControllers()
    .ConfigureJsonOptions()
    .ConfigureFluentValidation();

builder.Services.AddMvcCore(options => options.Filters.Add<ValidateModelFilterAttribute>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.OptionsValidate();

app.KafkaAdminClientConfigures();

app.UseApiCriticalExceptionHandling();
app.UseApiExceptionHandling();

app.UseRequestProcessing();

if (app.Environment.IsDevelopment() || app.Environment.IsLocal())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();