namespace MateMachine.CurrencyConverter.Business {
    public abstract class Graph<TNode, TEdge> {
        protected List<TNode> nodes = new List<TNode>();
        protected List<TEdge> edges = new List<TEdge>();

        public Graph() {
            nodes = new List<TNode>();
            edges = new List<TEdge>();
        }

        public void AddNode(TNode node) {
            this.nodes.Add(node);
        }

        public void AddNodes(IEnumerable<TNode> nodes) {
            this.nodes.AddRange(nodes);
        }

        public void AddEdge(TEdge edge) {
            this.edges.Add(edge);
        }

        public void AddEdges(IEnumerable<TEdge> edges) {
            this.edges.AddRange(edges);
        }

        protected abstract IEnumerable<TNode> GetNeighbours(TNode node);

        public List<TNode> GetShortestPath(TNode source, TNode destination) {
            // TODO: Optimization is required
            var previous = new Dictionary<TNode, TNode>();
            var queue = new Queue<TNode>();
            var path = new List<TNode>();

            queue.Enqueue(source);

            while (queue.Count > 0) {
                var v = queue.Dequeue();
                var neighbours = GetNeighbours(v);
                foreach (var neighbour in neighbours) {
                    if (previous.ContainsKey(neighbour))
                        continue;

                    previous[neighbour] = v;
                    queue.Enqueue(neighbour);
                    if (neighbour.Equals(destination))
                        break;
                }
            }


            var current = destination;
            while (!current.Equals(source)) {
                path.Add(current);
                if (!previous.ContainsKey(current)) {
                    return null;
                }

                current = previous[current];
            };

            path.Add(source);
            path.Reverse();

            return path;
        }
    }
}