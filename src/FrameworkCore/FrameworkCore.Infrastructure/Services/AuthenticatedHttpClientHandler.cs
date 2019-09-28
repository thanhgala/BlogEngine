﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FrameworkCore.Infrastructure.Services
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly Func<Task<string>> _getToken;

        public AuthenticatedHttpClientHandler(Func<Task<string>> getToken)
        {
            _getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth == null)
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            var token = await _getToken().ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        }
    }
}
