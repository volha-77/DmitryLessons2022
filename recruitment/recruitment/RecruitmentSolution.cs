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

            ActionExecuter submitActionExecuter = new ActionExecuter("SubmitForApproval", actionConfig);
            ActionExecuter approveActionExecuter = new ActionExecuter("Approve", actionConfig);
            ActionExecuter rejectActionExecuter = new ActionExecuter("Reject", actionConfig);

            List<IActionExecuter> actionExecuters = new List<IActionExecuter>()
            {
                submitActionExecuter,
                approveActionExecuter,
                rejectActionExecuter
            };


            var service = new Service(actionExecuters);
            // Invoke "Submit for Approval" for softwareEngineerVacancy
            service.ExecuteAction(softwareEngineerVacancy, "SubmitForApproval");
            // Invoke "Approve" for softwareEngineerVacancy
            service.ExecuteAction(softwareEngineerVacancy, "Approve");
            // Invoke "Submit for Approval" for frontEndEngineerVacancy
            service.ExecuteAction(frontEndEngineerVacancy, "SubmitForApproval");
            // Invoke "Reject" for frontEndEngineerVacancy
            service.ExecuteAction(frontEndEngineerVacancy, "Reject");
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


        public interface IActionExecuter
        {
            public string ActionType { get; }
            string GetActionResult(string recordType);
        }

        public class ActionExecuter : IActionExecuter
        {
            private string _actionType;

            public string ActionType { get { return _actionType; } }

            private ActionConfiguration _configuration { get; set; }

            public ActionExecuter(string actionType, ActionConfiguration configuration)
            {
                _actionType = actionType;
                _configuration = configuration;
            }

            public string GetActionResult(string recordType)
            {
                if (_configuration.ActionPerType.TryGetValue(recordType, out var actions) &&
                    (actions.TryGetValue(_actionType, out var result)))
                {
                    return result;
                }

                throw new InvalidOperationException();
            }
        }

        public class Service
        {
            private List<IActionExecuter> _ationExecutersList = new List<IActionExecuter>();

            public Service(List<IActionExecuter> ationExecutersList)
            {
                foreach (var item in ationExecutersList)
                {
                    _ationExecutersList.Add(item);
                }

            }

            public void ExecuteAction(Record record, string action)
            {
                foreach (var item in _ationExecutersList)
                {
                    if (item.ActionType.ToLower() == action.ToLower())
                    {
                        var actionResult = item.GetActionResult(record.Type);
                        Console.WriteLine($"{actionResult} {record.Type}: {record.Id}");

                        break;
                    }
                }
            }

        }

    }
}
