namespace BookingApp.Applications.Utilities.Validation
{
    public abstract class ValidationBase : NotifyPropertyChangedImplemented
    {
        public ValidationErrors ValidationErrors { get; set; }
        public bool IsValid { get; private set; }

        protected ValidationBase()
        {
            ValidationErrors = new ValidationErrors();
        }

        protected abstract void ValidateSelf();

        public void Validate()
        {
            ValidationErrors.Clear();
            ValidateSelf();
            IsValid = ValidationErrors.IsValid;
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ValidationErrors));
        }
    }
}
