using AutoMapper;

namespace ProjectUmico.Application.Mapings;

public static class MappingExtensions
{
    public static void ForAllMembersEnforceCustomDefaultValues<TReq,TRes>
        (this IMappingExpression<TReq,TRes> mappingExpression)
    {
        mappingExpression.ForAllMembers(o =>
        {
            o.Condition((c, p, requestMember, modelMember, resolutionCtx) =>
            {
                // if value is null then do not map. j is request k is model
                var sourceProp = typeof(TReq)
                    .GetProperty(o.DestinationMember.Name);
                var sourceType = sourceProp?.PropertyType as Type;
                    
                if (sourceType != null)
                {
                    if (requestMember is null || 
                        (sourceType == typeof(string) && requestMember.ToString() == "string"))
                    {
                        return false;
                    }
                    else if (sourceType == typeof(int?) && ((int?)requestMember) is null or 0)
                    {
                        return false;
                    }
                }   
                return true;
            });
        });
    }
}