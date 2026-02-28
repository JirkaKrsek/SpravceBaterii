namespace SpravceBaterii.Services
{
    public record ModalNavigationEntry(Type ComponentType, int? SelectedId);

    public class ModalNavigationService
    {
        public event Action? ModalChanged;
        private readonly List<ModalNavigationEntry> history = [];
        private bool reload = false;

        /// <summary>
        /// Získání komponenty, který má být zobrazena
        /// </summary>
        public ModalNavigationEntry? GetCurrentComponent()
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
            ModalChanged?.Invoke();
        }

        /// <summary>
        /// Při zavření komponenty se odstraní daná komponenta ze seznamu
        /// </summary>
        public void Close()
        {
            if (history.Count > 0)
            {
                history.RemoveAt(history.Count - 1);
                ModalChanged?.Invoke();
            }
        }

        /// <summary>
        /// Zavření všech otevřených komponent
        /// </summary>
        public void CloseAll()
        {
            history.Clear();
            ModalChanged?.Invoke();
        }

        /// <summary>
        /// Při zavření komponenty se odstraní daná komponenta ze seznamu
        /// Přidání komponenty k zobrazení
        /// </summary>
        /// <param name="componentType">Typ komponenty</param>
        /// <param name="id">ID vybrané položky, která má být zobrazena v komponentě / null, pokud žádná položka není vybraná</param>
        public void CloseAndOpen(Type componentType, int? id)
        {
            if (history.Count > 0)
            {
                history.RemoveAt(history.Count - 1);
            }

            history.Add(new ModalNavigationEntry(componentType, id));

            ModalChanged?.Invoke();
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
