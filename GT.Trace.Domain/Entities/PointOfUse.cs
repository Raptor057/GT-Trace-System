using System.Diagnostics;

namespace GT.Trace.Domain.Entities
{
    public sealed class PointOfUse
    {
        private readonly Dictionary<string, EtiList> _components = new();

        //Parte donde se valida si la eti pertenece al bom de materiales del numero cargado en la linea.
        public PointOfUse(string code, IEnumerable<BomComponent> bom, IEnumerable<SetComponent> set, Dictionary<string, List<string>>? loadedEtisByComponent)
        {
            Code = code;
            foreach (var comp in bom.Select(item => item.CompNo).Distinct())
            {
                var capacity = bom.Where(item => item.CompNo == comp).Sum(item => item.Capacity);
                //Debug.Print($"Component No.: {comp}");
                var activeEtiNo = set.SingleOrDefault(item => item.CompNo == comp)?.EtiNo;
                _components.Add(
                    comp,
                    new EtiList(
                        comp,
                        capacity,
                        activeEtiNo,
                        loadedEtisByComponent?.ContainsKey(comp) ?? false ? loadedEtisByComponent?[comp] : null));
            }
        }

        public string Code { get; }

        //public IReadOnlyDictionary<string, EtiList> Components => _components;

        public bool ComponentIsConfigured(string componentNo) => _components.ContainsKey(componentNo);

        public string? GetActiveEti(string componentNo) => _components.ContainsKey(componentNo) ? _components[componentNo].ActiveEtiNo : null;

        public bool ContainsEti(string etiNo) => _components.Any(c => c.Value.Contains(etiNo));

        public void UseMaterial(string etiNo) => _components.Single(c => c.Value.Contains(etiNo)).Value.Use(etiNo);

        public void LoadMaterial(Eti eti) => _components[eti.Component.Number].Add(eti.Number);

        public void UnloadMaterial(Eti eti) => _components[eti.Component.Number].Remove(eti.Number);

        public bool CheckIsFull(string componentNo) => _components[componentNo].Capacity <= _components[componentNo].Items.Count;
    }
}