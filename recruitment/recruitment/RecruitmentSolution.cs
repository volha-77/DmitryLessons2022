using System;
using System.Collections.Generic;

namespace recruitment
{
    class RecruitmentSolution
    {
        static void Main(string[] args)
        {
            String RecordType = "Vacancy";

            Record softwareEngineerVacancy = new Record("SoftwareEngineer0001", RecordType);
            Record frontEndEngineerVacancy = new Record("FrontEndEngineer0020", RecordType);

            IRecruitmentService service = new Service();

            /**
            * Implement a logic of actions and invoke those actions below:
            */

            // Invoke "Submit for Approval" for softwareEngineerVacancy
            service.SubmitApproval(softwareEngineerVacancy);
            // Invoke "Approve" for softwareEngineerVacancy
            service.Approve(softwareEngineerVacancy);
            // Invoke "Submit for Approval" for frontEndEngineerVacancy
            service.SubmitApproval(frontEndEngineerVacancy);
            // Invoke "Reject" for frontEndEngineerVacancy
            service.Reject(frontEndEngineerVacancy);

            // Console.ReadKey();
            //==========================
            Console.WriteLine("========service2==============");
            var config = new ServiceConfiguration
            {
                ActionPerType = new Dictionary<string, IDictionary<string, string>>
                {
                    { "Vacancy",   new Dictionary<string, string>
                        {
                            { "SubmitForApproval", "Submitted for approval" },
                            { "Approve", "Approved" },
                            { "Reject", "Rejected" }
                        }
                    }
                }
            };
            var service2 = new Service2(config);
            service2.ExecuteAction(softwareEngineerVacancy, "SubmitForApproval");
            service2.ExecuteAction(softwareEngineerVacancy, "Approve");
            service2.ExecuteAction(frontEndEngineerVacancy, "SubmitForApproval");
            service2.ExecuteAction(frontEndEngineerVacancy, "Reject");

            Console.WriteLine("========service3==============");
            var recruitmentExecuter = new RecruitmentExecuter(config);
            var service3 = new Service3();
            service3.RecruitmentExecuter = recruitmentExecuter;
            service3.RecruitmentExecuter.ExecuteAction(softwareEngineerVacancy, "SubmitForApproval");
            service3.RecruitmentExecuter.ExecuteAction(softwareEngineerVacancy, "Approve");
            service3.RecruitmentExecuter.ExecuteAction(frontEndEngineerVacancy, "SubmitForApproval");
            service3.RecruitmentExecuter.ExecuteAction(frontEndEngineerVacancy, "Reject");
        }

        public class Record
        {
            public String Id { get; private set; }
            public String Type { get; private set; }
            public Record(String Id, String Type)
            {
                this.Id = Id;
                this.Type = Type;
            }
        }

        public interface IRecruitmentService
        {
            void SubmitApproval(Record record);
            void Approve(Record record);
            void Reject(Record record);

        }

        public class Service : IRecruitmentService
        {
            public void SubmitApproval(Record record)
            {
                Console.WriteLine($"Submitted for approval Vacancy: {record.Id}");
            }
            public void Approve(Record record)
            {
                Console.WriteLine($"Approved Record: {record.Id}");
            }
            public void Reject(Record record)
            {
                Console.WriteLine($"Reject Record: {record.Id}");
            }

        }

        public class Service2
        {
            private ServiceConfiguration _configuration;

            public Service2(ServiceConfiguration configuration)
            {
                _configuration = configuration;
            }

            public void ExecuteAction(Record record, string action)
            {
                var actionResult = GetActionResult(action, record.Type);
                Console.WriteLine($"{actionResult} {record.Type}: {record.Id}");
            }

            private string GetActionResult(string action, string type)
            {
                if (_configuration.ActionPerType.TryGetValue(type, out var actions) &&
                    (actions.TryGetValue(action, out var result)))
                {
                    return result;
                }

                throw new InvalidOperationException();
            }
        }

        public class ServiceConfiguration
        {
            public IDictionary<string, IDictionary<string, string>> ActionPerType { get; set; }
        }

        //=========================Service3==============
        public class Service3
        {
            //private readonly IActionExecuter _recruitmentExecuter;

            //public Service3(IActionExecuter recruitmentExecuter)
            //{
            //    _recruitmentExecuter = recruitmentExecuter;
            //}

            public IActionExecuter RecruitmentExecuter { get; set; }
        }

        public interface IActionExecuter
        {
            void ExecuteAction(object obj, string action);
        }

        public class RecruitmentExecuter : IActionExecuter
        {
            private ServiceConfiguration _configuration;

            public RecruitmentExecuter(ServiceConfiguration configuration)
            {
                _configuration = configuration;
            }

            public void ExecuteAction(object obj, string action)
            {
                Record record = (Record)obj;
                var actionResult = GetActionResult(action, record.Type);
                Console.WriteLine($"{actionResult} {record.Type}: {record.Id}");
            }

            private string GetActionResult(string action, string type)
            {
                if (_configuration.ActionPerType.TryGetValue(type, out var actions) &&
                    (actions.TryGetValue(action, out var result)))
                {
                    return result;
                }

                throw new InvalidOperationException();
            }
        }
    }
}
