using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComplexCalculator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<SelectListItem> Operations { get; } = new List<SelectListItem>
        {
            new SelectListItem{Value="add", Text ="+"},
            new SelectListItem{Value="subtract", Text ="-"},
            new SelectListItem{Value="multiply", Text ="*"},
            new SelectListItem{Value="divide", Text ="/"}
        };
        [BindProperty]
        public string Operation { get; set; }
        [BindProperty]
        public string Num1 { get; set; } 
        [BindProperty]
        public string Num2 { get; set; } 
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            switch (Operation) { 
                case "add":
                    ViewData["Result"] = Complex.Parse(Num1) + Complex.Parse(Num2);
                    break;
                case "subtract":
                    ViewData["Result"] = Complex.Parse(Num1) - Complex.Parse(Num2);
                    break;
                case "multiply":
                    ViewData["Result"] = Complex.Parse(Num1) * Complex.Parse(Num2);
                    break;
                case "divide":
                    ViewData["Result"] = Complex.Parse(Num1) / Complex.Parse(Num2);
                    break;}
        }
    }
}
