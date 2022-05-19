using System;
using System.Collections.Generic;
using System.Linq;

namespace recruitment
{
    class RecruitmentSolution
    {
        static void Main(string[] args)
        {
            String RecordType = "Vacancy";

            Record softwareEngineerVacancy = new Record("SoftwareEngineer0001", RecordType);
            Record frontEndEngineerVacancy = new Record("FrontEndEngineer0020", RecordType);

            /**
            * Implement a logic of actions and invoke those actions below:
            */

            var actionConfig = new ActionConfiguration
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

            Console.WriteLine("==============version 1=====================");

            SubmitActionExecuter submitActionExecuter1 = new SubmitActionExecuter(actionConfig);
            ApproveActionExecuter approveActionExecuter1 = new ApproveActionExecuter(actionConfig);
            RejectActionExecuter rejectActionExecuter1 = new RejectActionExecuter(actionConfig);

            List<IActionExecuter> actionExecuters1 = new List<IActionExecuter>()
            {
                submitActionExecuter1,
                approveActionExecuter1,
                rejectActionExecuter1
            };

            var service1 = new Service(actionExecuters1);
            // Invoke "Submit for Approval" for softwareEngineerVacancy
            service1.ExecuteAction(softwareEngineerVacancy, "SubmitForApproval");
            // Invoke "Approve" for softwareEngineerVacancy
            service1.ExecuteAction(softwareEngineerVacancy, "Approve");
            // Invoke "Submit for Approval" for frontEndEngineerVacancy
            service1.ExecuteAction(frontEndEngineerVacancy, "SubmitForApproval");
            // Invoke "Reject" for frontEndEngineerVacancy
            service1.ExecuteAction(frontEndEngineerVacancy, "Reject");

            Console.WriteLine("==============version 2=====================");
            RecruitmentActionExecuter submitActionExecuter2 = new RecruitmentActionExecuter("SubmitForApproval", actionConfig);
            RecruitmentActionExecuter approveActionExecuter2 = new RecruitmentActionExecuter("Approve", actionConfig);
            RecruitmentActionExecuter rejectActionExecuter2 = new RecruitmentActionExecuter("Reject", actionConfig);

            List<IActionExecuter> actionExecuters2 = new List<IActionExecuter>()
            {
                submitActionExecuter2,
                approveActionExecuter2,
                rejectActionExecuter2
            };


            var service2 = new Service(actionExecuters2);
            // Invoke "Submit for Approval" for softwareEngineerVacancy
            service2.ExecuteAction(softwareEngineerVacancy, "SubmitForApproval");
            // Invoke "Approve" for softwareEngineerVacancy
            service2.ExecuteAction(softwareEngineerVacancy, "Approve");
            // Invoke "Submit for Approval" for frontEndEngineerVacancy
            service2.ExecuteAction(frontEndEngineerVacancy, "SubmitForApproval");
            // Invoke "Reject" for frontEndEngineerVacancy
            service2.ExecuteAction(frontEndEngineerVacancy, "Reject");
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

        public class ActionConfiguration
        {
            public IDictionary<string, IDictionary<string, string>> ActionPerType { get; set; }
        }

        public class ActionResult
        {
            public string TextResult { get; set; }

            public ActionResult(string textResult)
            {
                TextResult = textResult;
            }
        }

        public interface IActionExecuter
        {
            public string ActionType { get; }
            ActionResult GetActionResult(string recordType);
        }

        //classes for actions
        //version 1
        public class SubmitActionExecuter : IActionExecuter
        {
            public string ActionType { get; }

            private ActionConfiguration _configuration { get; set; }

            public SubmitActionExecuter(ActionConfiguration configuration)
            {
                ActionType = "SubmitForApproval";

                _configuration = configuration;
            }

            public ActionResult GetActionResult(string recordType)
            {
                if (_configuration.ActionPerType.TryGetValue(recordType, out var actions) &&
                    (actions.TryGetValue(ActionType, out var result)))
                {
                    return new ActionResult(result);
                }

                return new ActionResult("Submitted for approval ");
            }
        }

        public class ApproveActionExecuter : IActionExecuter
        {
            public string ActionType { get; }
            private ActionConfiguration _configuration { get; set; }

            public ApproveActionExecuter(ActionConfiguration configuration)
            {
                ActionType = "Approve";

                _configuration = configuration;
            }

            public ActionResult GetActionResult(string recordType)
            {
                if (_configuration.ActionPerType.TryGetValue(recordType, out var actions) &&
                     (actions.TryGetValue(ActionType, out var result)))
                {
                    return new ActionResult(result);
                }

                return new ActionResult("Approved ");
            }
        }

        public class RejectActionExecuter : IActionExecuter
        {
            public string ActionType { get; }

            private ActionConfiguration _configuration { get; set; }

            public RejectActionExecuter(ActionConfiguration configuration)
            {
                ActionType = "Reject";

                _configuration = configuration;
            }

            public ActionResult GetActionResult(string recordType)
            {
                if (_configuration.ActionPerType.TryGetValue(recordType, out var actions) &&
                    (actions.TryGetValue(ActionType, out var result)))
                {
                    return new ActionResult(result);
                }

                return new ActionResult("Rejected ");
            }
        }

        //version 2
        public class RecruitmentActionExecuter : IActionExecuter
        {
            private string _actionType;

            public string ActionType { get { return _actionType; } }

            private ActionConfiguration _configuration { get; set; }

            public RecruitmentActionExecuter(string actionType, ActionConfiguration configuration)
            {
                _actionType = actionType;
                _configuration = configuration;
            }

            public ActionResult GetActionResult(string recordType)
            {
                if (_configuration.ActionPerType.TryGetValue(recordType, out var actions) &&
                    (actions.TryGetValue(_actionType, out var result)))
                {
                    return new ActionResult(result);
                }

                throw new InvalidOperationException();
            }
        }

        //class servise
        public class Service
        {
            private List<IActionExecuter> _actionExecutersList = new List<IActionExecuter>();

            public Service(IEnumerable<IActionExecuter> ationExecutersList)
            {
                ationExecutersList.ToList().ForEach(x => _actionExecutersList.Add(x));

            }

            public void ExecuteAction(Record record, string action)
            {
                foreach (var item in _actionExecutersList)
                {
                    if (item.ActionType.ToLower() == action.ToLower())
                    {
                        var actionResult = item.GetActionResult(record.Type);
                        Console.WriteLine($"{actionResult.TextResult} {record.Type}: {record.Id}");

                        break;
                    }
                }
            }

        }

    }
}
