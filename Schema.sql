-- ===============================
-- Criação de Tabelas
-- ===============================

CREATE TABLE hosts (
    id SERIAL PRIMARY KEY,
    name VARCHAR(25) NOT NULL,
    operating_system VARCHAR(40) NOT NULL,
    ip_address VARCHAR(15) NOT NULL,
    architecture VARCHAR(10) NOT NULL,
    isActive BOOLEAN NOT NULL,
    uniqueId VARCHAR(36) NOT NULL UNIQUE,
    port INTEGER NOT NULL DEFAULT 5643,
    description VARCHAR(225) NOT NULL,
    location VARCHAR(125) NOT NULL
);

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(60) NOT NULL,
    username VARCHAR(25) NOT NULL,
    email VARCHAR(125) NOT NULL,
    isAdmin BOOLEAN NOT NULL DEFAULT FALSE,
    createdAt TIMESTAMP NOT NULL DEFAULT NOW(),
    isActive BOOLEAN DEFAULT TRUE
);

CREATE TABLE tokens (
    id SERIAL PRIMARY KEY,
    hostId INTEGER,
    userId INTEGER,
    createAt TIMESTAMP NOT NULL DEFAULT NOW(),
    isActive BOOLEAN DEFAULT TRUE,
    expiresIn TIMESTAMP,
    hash VARCHAR(256) NOT NULL,
    type VARCHAR(30) NOT NULL,
    CONSTRAINT fk_tokens_host FOREIGN KEY (hostId) REFERENCES hosts(id) ON DELETE CASCADE,
    CONSTRAINT fk_tokens_user FOREIGN KEY (userId) REFERENCES users(id) ON DELETE CASCADE
);

CREATE INDEX idx_tokens_hostId ON tokens(hostId);
CREATE INDEX idx_tokens_userId ON tokens(userId);

CREATE TABLE docker_config (
    id SERIAL PRIMARY KEY,
    name VARCHAR(32) NOT NULL,
    hostPorts VARCHAR(32) NOT NULL,
    runParams VARCHAR(32),
    containerName VARCHAR(32) NOT NULL,
    image VARCHAR(32) NOT NULL,
    env_vars TEXT,
    volumes VARCHAR(150)
);

CREATE TABLE projects (
    id SERIAL PRIMARY KEY,
    name VARCHAR(25) NOT NULL,
    description VARCHAR(125),
    createdAt TIMESTAMP,
    status INTEGER, -- Active, Archived, Online
    ownerId INTEGER NOT NULL
);

CREATE TABLE repositories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(40) NOT NULL,
    solutionId INTEGER NOT NULL,
    repositoryUrl VARCHAR(120) NOT NULL,
    branch VARCHAR(20) NOT NULL DEFAULT 'main',
    targetPath VARCHAR(100) NOT NULL,
    hostId INTEGER NOT NULL,
    status INTEGER NOT NULL,
    lastSync TIMESTAMP,
    lastClone TIMESTAMP,
    lastPush TIMESTAMP,
    last_commit_hash VARCHAR(50),
    autoSync BOOLEAN DEFAULT FALSE,
    isDockerEnabled BOOLEAN DEFAULT FALSE,
    dockerConfigId INTEGER NOT NULL,
    CONSTRAINT fk_repo_solution FOREIGN KEY (solutionId) REFERENCES projects(id),
    CONSTRAINT fk_repo_host FOREIGN KEY (hostId) REFERENCES hosts(id),
    CONSTRAINT fk_repo_docker FOREIGN KEY (dockerConfigId) REFERENCES docker_config(id)
);

CREATE INDEX idx_repository_solution ON repositories(solutionId);
CREATE INDEX idx_repository_host ON repositories(hostId);
CREATE INDEX idx_docker_config_repository ON repositories(dockerConfigId);

CREATE TABLE workflows (
    id SERIAL PRIMARY KEY,
    name VARCHAR(32) NOT NULL,
    command VARCHAR(225) NOT NULL,
    description VARCHAR(125),
    iconUrl VARCHAR(225),
    isLinux BOOLEAN NOT NULL
);

CREATE TABLE project_workflow (
    id SERIAL PRIMARY KEY,
    projectId INTEGER NOT NULL,
    workflowId INTEGER NOT NULL,
    executionOrder INTEGER NOT NULL,
    stageName VARCHAR(20),
    CONSTRAINT fk_project_workflow_project FOREIGN KEY (projectId) REFERENCES projects(id),
    CONSTRAINT fk_project_workflow_workflow FOREIGN KEY (workflowId) REFERENCES workflows(id) ON DELETE CASCADE
);

CREATE INDEX idx_project_workflow_projectId ON project_workflow(projectId);
CREATE INDEX idx_project_workflow_workflowId ON project_workflow(workflowId);

CREATE TABLE host_services (
    id SERIAL PRIMARY KEY,
    hostId INTEGER NOT NULL,
    service_name VARCHAR(50) NOT NULL,
    port INTEGER,
    status VARCHAR(20) NOT NULL,
    last_check TIMESTAMP,
    path TEXT,
    workdir TEXT,
    CONSTRAINT fk_host_services_host FOREIGN KEY (hostId) REFERENCES hosts(id)
);

CREATE TABLE host_metrics (
    id SERIAL PRIMARY KEY,
    hostId INTEGER NOT NULL,
    timestamp TIMESTAMP NOT NULL,
    cpu_usage DECIMAL(5,2),
    memory_usage_mb INTEGER,
    disk_usage_gb DECIMAL(5,2),
    network_in_mb DECIMAL(10,2),
    network_out_mb DECIMAL(10,2),
    CONSTRAINT fk_metrics_host FOREIGN KEY (hostId) REFERENCES hosts(id) ON DELETE CASCADE
);

CREATE INDEX idx_metrics_hostId ON host_metrics(hostId);
CREATE INDEX idx_metrics_timestamp ON host_metrics(timestamp);

CREATE TABLE audit_logs (
    id SERIAL PRIMARY KEY,
    userId INTEGER,
    action VARCHAR(100) NOT NULL,
    details JSON,
    ip VARCHAR(15),
    timestamp TIMESTAMP NOT NULL DEFAULT NOW(),
    CONSTRAINT fk_audit_logs_user FOREIGN KEY (userId) REFERENCES users(id)
);

CREATE TABLE deployments (
    id SERIAL PRIMARY KEY,
    repositoryId INTEGER NOT NULL,
    workflowId INTEGER,
    status VARCHAR(20) NOT NULL,
    startedAt TIMESTAMP,
    finishedAt TIMESTAMP,
    triggeredBy INTEGER NOT NULL,
    CONSTRAINT fk_deployments_repository FOREIGN KEY (repositoryId) REFERENCES repositories(id) ON DELETE CASCADE
);

CREATE INDEX idx_deployment_repository ON deployments(repositoryId);

-- ===============================
-- OBS: removi a referência inválida
-- "repositories"."autoSync" < "deployments"."finishedAt"
-- porque não faz sentido FK entre BOOLEAN e TIMESTAMP.
-- ===============================
