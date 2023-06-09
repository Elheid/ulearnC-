using System;
using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    public string GetDescription(Type value)
    {
        var attribute = value.GetCustomAttributes(false).OfType<ApiDescriptionAttribute>().FirstOrDefault();
        return attribute == null ? null : attribute.Description;
    }

    public string GetDescription(MethodInfo value)//Нужно как-то убрать повторение
    {
        var attribute = value.GetCustomAttributes(false).OfType<ApiDescriptionAttribute>().FirstOrDefault();
        return attribute == null ? null : attribute.Description;
    }

    public string GetDescription(ParameterInfo value)//по сути методы одинаковые просто принимают разные значения
    {
        var attribute = value.GetCustomAttributes(false).OfType<ApiDescriptionAttribute>().FirstOrDefault();
        return attribute == null ? null : attribute.Description;
    }

    public bool IsMethodInvalid(MethodInfo method) =>
        method == null || !(method.CustomAttributes.OfType<ApiMethodAttribute>().Count() == 0);

    public ApiParamDescription GetApiParamDescription
        (string methodName, string paramName, ApiParamDescription exception)
    {
        var param = new ApiParamDescription();
        param.ParamDescription = new CommonDescription(paramName);
        var method = typeof(T).GetMethod(methodName);
        if (IsMethodInvalid(method))
            return exception;
        var parameters = method.GetParameters()
            .Where(param => param.Name == paramName);
        if (parameters.Count() == 0)
            return exception;
        return GetParamDescription(parameters.First(), param.ParamDescription);
    }

    public void AssignMaxAndMin(ApiParamDescription result, ParameterInfo parameter)
    {
        var intAttribute = parameter.GetCustomAttributes(false).OfType<ApiIntValidationAttribute>().FirstOrDefault();
        if (intAttribute != null)
        {
            result.MinValue = intAttribute.MinValue;
            result.MaxValue = intAttribute.MaxValue;
        }
    }

    public ApiParamDescription GetParamDescription
        (ParameterInfo parameter, CommonDescription deafultDescription)
    {
        var param = new ApiParamDescription();
        param.ParamDescription = deafultDescription;
        var requiredAttribute = parameter.GetCustomAttributes(false).OfType<ApiRequiredAttribute>().FirstOrDefault();
        param.ParamDescription.Description = GetDescription(parameter);
        AssignMaxAndMin(param, parameter);
        if (requiredAttribute != null)
            param.Required = requiredAttribute.Required;
        return param;
    }

    public string GetApiDescription()
    {
        return GetDescription(typeof(T));
    }

    public string[] GetApiMethodNames()
    {
        return typeof(T).GetMethods()
            .Where(method => method.IsPublic && method.GetCustomAttributes(false).OfType<ApiMethodAttribute>().Count() > 0)
            .Select(method => method.Name)
            .ToArray();
    }

    public string GetApiMethodDescription(string methodName)
    {
        var method = typeof(T).GetMethod(methodName);
        var invalidMethod = method == null || !(method.CustomAttributes.OfType<ApiMethodAttribute>().Count() == 0);
        return IsMethodInvalid(method) ? null : GetDescription(method);
    }

    public string[] GetApiMethodParamNames(string methodName)
    {
        var method = typeof(T).GetMethod(methodName);
        return method.GetParameters()
            .Select(p => p.Name)
            .ToArray();
    }

    public string GetApiMethodParamDescription(string methodName, string paramName)
    {
        var res = GetApiParamDescription(methodName, paramName, null);
        return res == null ? null : res.ParamDescription.Description;
    }

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var param = new ApiParamDescription();
        param.ParamDescription = new CommonDescription(paramName);
        return GetApiParamDescription(methodName, paramName, param);
    }

    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        var method = typeof(T).GetMethod(methodName);
        if (method.GetCustomAttributes(false).OfType<ApiMethodAttribute>().Count() == 0)
            return null;
        if (IsMethodInvalid(method))
            return null;
        var result = new ApiMethodDescription();
        result.MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName));
        result.ParamDescriptions = GetApiMethodParamNames(methodName).Select(paramName => GetApiMethodParamFullDescription(methodName, paramName)).ToArray();
        var param = GetParamDescription(method.ReturnParameter, new CommonDescription());
        var requiredAttribute = method.ReturnParameter.GetCustomAttributes(false).OfType<ApiRequiredAttribute>().FirstOrDefault();
        if (requiredAttribute != null)
            result.ReturnDescription = param;
        return result;
    }
}