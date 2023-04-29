using System.Text;

namespace UserManager.MVC.Models;

public class ValidationErrorsModel
{
    private List<string> Errors { get; }

    public ValidationErrorsModel(List<string> errors)
    {
        Errors = errors;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendJoin(Environment.NewLine, Errors);
        return sb.ToString();
    }
}