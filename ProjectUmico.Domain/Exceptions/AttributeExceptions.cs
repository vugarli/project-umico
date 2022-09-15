namespace ProjectUmico.Domain.Exceptions;

public static class AttributeExceptions
{
    public class GroupAttributeCantHaveParentException : Exception
    {
        public GroupAttributeCantHaveParentException(string message = nameof(GroupAttributeCantHaveParentException)) : base(message) { }
    }
    public class AttributeCantHaveAttributeParentException : Exception
    {
        public AttributeCantHaveAttributeParentException(string message = nameof(AttributeCantHaveAttributeParentException)) : base(message) { }
    }
    public class AttributeMustHaveParentException : Exception
    {
        public AttributeMustHaveParentException(string message = nameof(AttributeMustHaveParentException)) : base(message) { }
    }
}