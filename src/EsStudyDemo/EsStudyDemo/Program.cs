using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using EsStudyDemo.Models;
using Nest;

namespace EsStudyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //连接配置-单机
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("people");
            ////连接池-集群配置
            //var uris = new[]
            //{
            //    new Uri("http://localhost:9200"),
            //    new Uri("http://localhost:9201"),
            //    new Uri("http://localhost:9202"),
            //};

            //var connectionPool = new SniffingConnectionPool(uris);
            //var settings = new ConnectionSettings(connectionPool)
            //    .DefaultIndex("people");
            var client = new ElasticClient(settings);
            var response = IndexPerson(client).Result;
            Console.Write(response.Index);
            Console.ReadLine();
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        static async Task<IIndexResponse> IndexPerson(IElasticClient client)
        {
            var person = new Person() { Id=1,Age = 20,FirstName = "m",LastName = "h"};
            return await client.IndexDocumentAsync(person);
        }
        /// <summary>
        /// 按条件查找
        /// </summary>
        /// <param name="client"></param>
        /// <param name="firstName"></param>
        /// <returns></returns>
        static async Task<List<Person>> GetPersonByFirstName(IElasticClient client,string firstName)
        {
            var searchRespone = await client.SearchAsync<Person>(c=>c.From(0).Size(10).Query(x=>x.Match(m=>m.Field(y=>y.FirstName).Query(firstName))));
            return searchRespone.Documents.ToList();
        }
    }
}
