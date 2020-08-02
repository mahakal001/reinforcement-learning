from torch import randint
import gym

rew_arr = []
episode_count = 100
env = gym.make('CartPole-v0')
for i in range(episode_count):
    obs, done, rew = env.reset(), False, 0
    while done != True:
        A = randint(0, env.action_space.n, (1,))
        obs, reward, done, info = env.step(A.item())
        rew += reward
    rew_arr.append(rew)

print("average reward per episode :", sum(rew_arr) / len(rew_arr))
