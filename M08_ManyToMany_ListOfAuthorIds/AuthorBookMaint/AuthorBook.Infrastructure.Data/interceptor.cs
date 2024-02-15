using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorBook.Infrastructure.Data;

public class MyMaterializationInterceptor : IMaterializationInterceptor
{
    public InterceptionResult<object> CreatingInstance(
        MaterializationInterceptionData materializationData,
        InterceptionResult<object> result) => result;

    public object CreatedInstance(
        MaterializationInterceptionData materializationData,
        object entity) => entity;

    public InterceptionResult InitializingInstance(
        MaterializationInterceptionData materializationData,
        object entity, InterceptionResult result) => result;

    public object InitializedInstance(
        MaterializationInterceptionData materializationData,
        object entity) => entity;

}