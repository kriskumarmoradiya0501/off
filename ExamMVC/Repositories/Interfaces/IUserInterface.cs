namespace ExamMVC;

public interface IUserInterface
{
    Task<List<t_User4>> GetAll();

    Task<t_User4?> GetById(int id);
    Task<int> Add(t_User4 user);

    Task<int>Update(t_User4 user);

    Task<int> Delete(int id);

    Task<t_User4?> Login(string email, string password);
}
