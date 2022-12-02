namespace UnitTest;

[TestClass]
public class JobTest
{
    [Fact]
    public async Task CreateJob()
    {
        var serviceProvider = BuildServiceProvider();
        var jobService = serviceProvider.GetRequiredService<JobService>();
        var result = await jobService.CreateJobAsync("Test", "Address", "Description", 10, 20, DateTime.Now);
        Assert.True(result.Success);
    }


    [Fact]
    public async Task CreateApplicant()
    {
        var serviceProvider = BuildServiceProvider();
        var jobService = serviceProvider.GetRequiredService<JobService>();
        var result = await jobService.CreateJobAsync("Test", "Address", "Description", 10, 20, DateTime.Now);
        Assert.True(result.Success);
    }

}