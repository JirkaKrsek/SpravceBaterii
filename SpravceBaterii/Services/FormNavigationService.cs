namespace SpravceBaterii.Services
{
    public record FormNavigationEntry(Type ComponentType, int? SelectedId);

    public class FormNavigationService
    {
        private readonly List<FormNavigationEntry> history = [];
        private bool reload = false;

        /// <summary>
        /// Získání formuláře, který má být zobrazen
        /// </summary>
        public FormNavigationEntry? GetCurrentForm()
        {
            return history.LastOrDefault();
        }

        /// <summary>
        /// Přidání komponenty k zobrazení
        /// </summary>
        /// <param name="formType">Typ formulářové komponenty</param>
        /// <param name="selectedId">ID vybrané položky, která má být zobrazena v komponentě / null, pokud žádná položka není vybraná</param>
        public void Open(Type formType, int? selectedId)
        {
            history.Add(new FormNavigationEntry(formType, selectedId));
        }

        /// <summary>
        /// Při zavření komponenty se odstraní daná komponenta ze seznamu
        /// </summary>
        public void Close()
        {
            if (history.Count > 0)
            {
                history.RemoveAt(history.Count - 1);
            }
        }

        /// <summary>
        /// Nastavení reloadList hodnoty na true
        /// </summary>
        public void MarkForReload()
        {
            reload = true;
        }

        /// <summary>
        /// Rozhodnutí, zda se mají data na stránce znovu načíst
        /// </summary>
        /// <returns>bool, zda se mají data znovu načíst</returns>
        public bool ShouldReload()
        {
            if (history.Count < 1)
            {
                return reload;
            }
            return false;
        }

        /// <summary>
        /// Nastavení na výchozí hodnoty
        /// </summary>
        public void Reset()
        {
            history.Clear();
            reload = false;
        }
    }
}
