namespace ExamMVC;

public interface IDepartmentInterface
{
    Task<List<dynamic>> GetAll();
}
