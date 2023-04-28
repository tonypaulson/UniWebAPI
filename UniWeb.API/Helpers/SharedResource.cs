using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Localization;

namespace UniWeb.API.Helpers
{
    public class SharedResource : ISharedResource
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SharedResource(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public string UnknownError => _localizer["UnknownError"];

        public string SaveSuccess => _localizer["SaveSuccess"];

        public string DeleteSuccess => _localizer["DeleteSuccess"];

        public string DocumentDeleteSuccess => _localizer["DocumentDeleteSuccess"];
        public string ServiceDeleteSuccess => _localizer["ServiceDeleteSuccess"];

        public string NoteDeleteSuccess => _localizer["NoteDeleteSuccess"];

        public string UserEmailExists => _localizer["UserEmailExists"];

        public string CategoryExists => _localizer["CategoryExists"];

        public string CategoryUsedByServices => _localizer["CategoryUsedByServices"];

        public string ServiceNameExists => _localizer["ServiceNameExists"];

        public string TokenIdNotPresent => _localizer["TokenIdNotPresent"];

        public string InvalidMeetingCode => _localizer["InvalidMeetingCode"];

        public string VideoConferenceInviteEmailTemplate => _localizer["VideoConferenceInviteEmailTemplate"];

        public string UserActivationEmailTemplate => _localizer["UserActivationEmailTemplate"];
        public string ForgetPasswordEmail => _localizer["ForgetPasswordEmail"];

        public string AppointmentEmailTemplate => _localizer["AppointmentEmailTemplate"];

        public string PatientEmailExists => _localizer["PatientEmailUsed"];

        public string StaffEmailExists => _localizer["StaffEmailUsed"];

        public string TenantWelcomeEmailTemplate => _localizer["TenantWelcomeEmailTemplate"];

        public string StaffVideoConferenceEmailTemplate => _localizer["StaffVideoConferenceEmailTemplate"];
        public string PatientDeleteSuccess => _localizer["Patient Deleted"];
        public string QuestionnaireDeleteSuccess => _localizer["Questionnaire Deleted"];
        public string CategoryDeleteSuccess => _localizer["Category Deleted"];

        public string QuestionnaireCategoryExists => _localizer["CategoryExists"];

        public string QuestionnaireExists => _localizer["QuestionnaireExists"];
        public string QuestionnaireNotPresent => _localizer["Not uploaded any questionnaire"];
        public string CategoryUsedByQuestionnaire => _localizer["The category is used by a questionniare"];
        public string DocumentExists => _localizer["Document with the same name exists"];

    }
}
