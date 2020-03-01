using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private MockResponse response;
        private MockRequest request;
        private HttpCookieCollection cookies;

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            response = new MockResponse(cookies);
            request = new MockRequest(cookies);
        }


        //Override Response Property
        public override HttpResponseBase Response
        {
            get
            {
                return response;
            }
        }

        //Override Request Property
        public override HttpRequestBase Request
        {
            get
            {
                return request;
            }
        }


    }

}
/// <summary>
/// 
/// </summary>
public class MockResponse : HttpResponseBase
{
    private readonly HttpCookieCollection cookies;

    public MockResponse(HttpCookieCollection cookies)
    {
        this.cookies = cookies;
    }

    public override HttpCookieCollection Cookies
    {
        get
        {
            return cookies;
        }
    }

}
/// <summary>
/// 
/// </summary>
public class MockRequest : HttpRequestBase
{
    private readonly HttpCookieCollection cookies;

    public MockRequest(HttpCookieCollection cookies)
    {
        this.cookies = cookies;
    }

    public override HttpCookieCollection Cookies
    {
        get
        {
            return cookies;
        }
    }

}



