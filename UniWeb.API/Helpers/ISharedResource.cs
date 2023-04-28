namespace UniWeb.API.Helpers
{
    public interface ISharedResource
    {
        string UnknownError { get; }

        string SaveSuccess { get; }

        string DeleteSuccess { get; }

        string DocumentDeleteSuccess { get; }

        string NoteDeleteSuccess { get; }
        string ServiceDeleteSuccess { get; }

        string UserEmailExists { get; }
        string PatientEmailExists { get; }
        string StaffEmailExists { get; }

        string CategoryExists { get; }

        string CategoryUsedByServices { get; }

        string ServiceNameExists { get; }

        string TokenIdNotPresent { get; }

        string InvalidMeetingCode { get; }

        string VideoConferenceInviteEmailTemplate { get; }
        string AppointmentEmailTemplate { get; }

        string UserActivationEmailTemplate { get; }

        string ForgetPasswordEmail { get; }
        string TenantWelcomeEmailTemplate { get; }
        string StaffVideoConferenceEmailTemplate { get; }
        string PatientDeleteSuccess { get; }
        string CategoryDeleteSuccess { get; }
        string QuestionnaireDeleteSuccess { get; }

        string QuestionnaireCategoryExists { get; }
        string QuestionnaireExists { get; }
        string QuestionnaireNotPresent { get; }
        string CategoryUsedByQuestionnaire { get; }

        string DocumentExists { get; }
    }
}
