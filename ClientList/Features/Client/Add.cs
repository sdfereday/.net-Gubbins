using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MediatR;

namespace ClientList.Features.Client
{
    public class Add
    {
        
    }

    public class AddClientQuery : IRequest<AddClientCommand>
    {

    }

    public class AddClientResult
    {

    }

    public class AddClientCommand : IRequest<AddClientResult>()
    {

    }
}
