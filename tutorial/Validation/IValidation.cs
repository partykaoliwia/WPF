using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Validation
{
    public interface IValidation
    {
        string Name { get; }
        string Description { get; }
    }
}
