namespace SpravceBaterii.Services
{
    public record ModalNavigationEntry(Type ComponentType, int? SelectedId);

    public class ModalNavigationService
    {
        private readonly List<ModalNavigationEntry> history = [];
        private bool reload = false;

        /// <summary>
        /// Získání formuláře, který má být zobrazen
        /// </summary>
        public ModalNavigationEntry? GetCurrentForm()
        {
            return history.LastOrDefault();
        }

        /// <summary>
        /// Přidání komponenty k zobrazení
        /// </summary>
        /// <param name="componentType">Typ komponenty</param>
        /// <param name="selectedId">ID vybrané položky, která má být zobrazena v komponentě / null, pokud žádná položka není vybraná</param>
        public void Open(Type componentType, int? selectedId)
        {
            history.Add(new ModalNavigationEntry(componentType, selectedId));
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
        /// Zavření všech otevřených komponent
        /// </summary>
        public void CloseAll()
        {
            history.Clear();
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
