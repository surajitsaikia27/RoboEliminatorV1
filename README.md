# RoboEliminartorV1
In this project, I have created a 3D environment in the Unity-3D platform, where a robot learns to eliminate humanoid robots using reinforcement learning and Unity-ML. The robot observes the surrounding environment and learns based on a reward function that penalizes the robot if it falls from the plane and gives a reward for every elimination. The update policy used for training the robot is Proximal Policy Optimization. 

#### Installing Unity-ML 
Follow this link to install Unity-3D and Unity Machine learning 
https://github.com/Unity-Technologies/ml-agents

#### Import the repository to the Assets folder
After following the instruction in the above link, please import the contents of this repository inside the Assets folder

#### Training script using Proximal policy optimization

```yaml
behaviors:
    Robot:
        trainer_type: ppo
        hyperparameters:
            batch_size: 128
            buffer_size: 2048
            learning_rate: 0.0003
            beta: 0.01
            epsilon: 0.2
            lambd: 0.95
            num_epoch: 3
            learning_rate_schedule: linear
        network_settings:
            normalize: false
            hidden_units: 256
            num_layers: 2
            vis_encode_type: simple
        reward_signals:
            extrinsic:
                gamma: 0.99
                strength: 1.0
        keep_checkpoints: 5
        max_steps: 1000000
        time_horizon: 128
        summary_freq: 5000
        threaded: true

```

*Note*: I am still working in this project, so I am updating the repository frequently.
