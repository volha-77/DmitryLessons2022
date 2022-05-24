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

            //version 1
            SubmitActionExecuter submitActionExecuter = new SubmitActionExecuter(actionConfig);
            ApproveActionExecuter approveActionExecuter = new ApproveActionExecuter(actionConfig);
            //version 2
            RecruitmentActionExecuter rejectActionExecuter = new RecruitmentActionExecuter("Reject", actionConfig);


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

            service.ExecuteAction(frontEndEngineerVacancy, "111");

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

            public bool Result;

            public ActionResult(string textResult, bool result = true)
            {
                TextResult = textResult;

                Result = result;
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

                //return new ActionResult(new InvalidOperationException().Message, false);

                return new ActionResult("", false);
            }
        }

        //class servise
        public class Service
        {
            private List<IActionExecuter> _actionExecutersList;

            private IDictionary<string, IActionExecuter> _searchDict = new Dictionary<string, IActionExecuter>();

            public Service(IEnumerable<IActionExecuter> ationExecutersList)
            {
                _actionExecutersList = ationExecutersList.ToList();
                foreach (IActionExecuter item in _actionExecutersList)
                {
                    _searchDict.Add(item.ActionType, item);
                }
            }

            public void ExecuteAction(Record record, string action)
            {
                _searchDict.TryGetValue(action, out var executer);
              //  var executer = _actionExecutersList.Find(x => x.ActionType.ToLower() == action.ToLower());
                if (executer != null)
                {
                    var actionResult = executer.GetActionResult(record.Type);
                    if (actionResult.Result)
                    {
                        Console.WriteLine($"{actionResult.TextResult} {record.Type}: {record.Id}");
                    }
                    else Console.WriteLine("Action failed");
                }
            }

        }

    }
}
