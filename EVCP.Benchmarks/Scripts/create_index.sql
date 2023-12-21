-- edge
CREATE INDEX IF NOT EXISTS idx_edge_start_node_id on edge(start_node_id, end_node_id);
--CREATE INDEX IF NOT EXISTS idx_edge_edge_info_id on edge(edge_info_id);