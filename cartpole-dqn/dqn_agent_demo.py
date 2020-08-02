import gym
from tqdm import tqdm
from time import sleep
from dqn_agent import DQN_Agent

env = gym.make('CartPole-v0')
env = gym.wrappers.Monitor(env, "record_dir", force='True')

input_dim = env.observation_space.shape[0]
output_dim = env.action_space.n
exp_replay_size = 256
agent = DQN_Agent(seed=1423, layer_sizes=[input_dim, 64, output_dim], lr=1e-3, sync_freq=5,
                  exp_replay_size=exp_replay_size)
agent.load_pretrained_model("cartpole-dqn.pth")

reward_arr = []
for i in tqdm(range(100)):
    obs, done, rew = env.reset(), False, 0
    while not done:
        A = agent.get_action(obs, env.action_space.n, epsilon=0)
        obs, reward, done, info = env.step(A.item())
        rew += reward
        # sleep(0.01)
        # env.render()

    reward_arr.append(rew)
print("average reward per episode :", sum(reward_arr) / len(reward_arr))
