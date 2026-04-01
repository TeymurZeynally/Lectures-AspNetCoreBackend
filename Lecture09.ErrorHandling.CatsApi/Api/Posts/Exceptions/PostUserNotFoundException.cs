namespace Lecture09.ErrorHandling.CatsApi.Api.Posts.Exceptions
{
    public class PostUserNotFoundException(string message) : Exception(message)
    {
    }
}
