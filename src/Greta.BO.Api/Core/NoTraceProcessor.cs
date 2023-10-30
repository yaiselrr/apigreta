

namespace Greta.BO.Api.Core;

// public class NoTraceProcessor: BaseProcessor<Activity>
// {
//     private readonly Predicate<string> _predicate;
//
//     public NoTraceProcessor(Predicate<string> predicate)
//     {
//         _predicate = predicate;
//     }
//
//     public override void OnEnd(Activity activity)
//     {
//         if (_predicate(activity.DisplayName))
//         {
//             activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
//         }
//     }
// }