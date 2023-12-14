-- edge_info
CREATE INDEX IF NOT EXISTS idx_edge_info_osm_way_id on edge_info(osm_way_id);

-- node
CREATE INDEX IF NOT EXISTS idx_node_osm_node_id on node(osm_node_id);
CREATE INDEX IF NOT EXISTS idx_node_gps_coords ON node
  USING GIST (gps_coords);

-- edge
CREATE INDEX IF NOT EXISTS idx_edge_start_node_id on edge(start_node_id);
CREATE INDEX IF NOT EXISTS idx_edge_end_node_id on edge(end_node_id);
CREATE INDEX IF NOT EXISTS idx_edge_edge_info_id on edge(edge_info_id);