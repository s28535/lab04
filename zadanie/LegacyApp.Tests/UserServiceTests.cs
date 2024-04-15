namespace LegacyApp.Tests;

public class UserServiceTests
{
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Without_At_And_Dot()
    {
        var service = new UserService();

        bool result = service.AddUser("John", "Doe", "doe", new DateTime(1980, 1, 1), 1);
        
        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_First_Name_And_Last_Name_Is_Null()
    {
        var service = new UserService();

        bool result = service.AddUser(null, null, "doe@doe.doe", new DateTime(1980, 1, 1), 1);
        
        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_First_Name_And_Last_Name_Is_Empty()
    {
        var service = new UserService();

        bool result = service.AddUser("", "", "doe@doe.doe", new DateTime(1980, 1, 1), 1);
        
        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Age_Under_21()
    {
        var service = new UserService();

        bool result = service.AddUser("John", "Doe", "doe@doe.doe", new DateTime(DateTime.Now.Year-20, 1, 1), 1);
        
        Assert.Equal(false, result);
    }

    [Fact]
    public void CalculateAge_Should_Return_50()
    {
        var service = new UserService();

        int result = service.CalculateAge(new DateTime(DateTime.Now.Year - 50, 1, 1));
        
        Assert.Equal(50, result);
    }
}