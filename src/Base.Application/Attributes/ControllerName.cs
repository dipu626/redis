using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Base.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerNameAtrribute : Attribute
    {
        public string Name { get; }

        public ControllerNameAtrribute(string name)
        {
            Name = name;
        }
    }

    public class ControllerNameAttributeConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNameAttribute = controller.Attributes.OfType<ControllerNameAtrribute>().SingleOrDefault();

            if (controllerNameAttribute is not null)
            {
                controller.ControllerName = controllerNameAttribute.Name;
            }
        }
    }
}
