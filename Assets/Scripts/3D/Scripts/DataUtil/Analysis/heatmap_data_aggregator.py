import os
import sys

rootdir = "../../../../../../CrystallizeRemote/PlayerData"

commands = {
	'pos': {
		'name': 'pos',
		'out_file': './positions.json',
		'scale': 5,
		'probability': False
	},
	'chat': {
		'name': 'chat',
		'out_file': './chat_positions.json',
		'scale': 0.5,
		'probability': False
	},
	'quit': {
		'name': 'quit',
		'out_file': './quit_positions.json',
		'scale': 0.5,
		'probability': False
	},
	'found': {
		'name': 'found',
		'out_file': './found_positions.json',
		'scale': 1,
		'probability': False
	},
	'weighted_found': {
		'name': 'weighted_found',
		'out_file': './weighted_found_positions.json',
		'scale': 1,
		'probability': False
	},
	'prob_pos': {
		'name': 'prob_pos',
		'out_file': './prob_positions.json',
		'scale': 4,
		'probability': True
	}
}

max_x = -999999
max_y = -999999
min_x = 999999
min_y = 999999

all_coords = []

def pos_from_line(s):
	return (float(s[s.find("(")+1:s.find(",")]), float(s[s.rfind(",")+1:s.find(")")]))

def process_files(command, subdir, files):
	global max_x, max_y, min_x, min_y, all_coords
	for file in files:
			f = open(os.path.join(subdir, file), 'r')
			lines = f.readlines()
			f.close()

			coords = []
			prev_pos = None
			if command['name'] == 'pos' or command['name'] == 'prob_pos':
				pos_lines = [x for x in lines if "Position" in x]
				pos_lines = map(pos_from_line, pos_lines)
				threshold = 1000
				coords = [c for c in pos_lines if c[0] * c[0] + c[1] * c[1] > threshold]
			elif command['name'] == 'chat':
				for line in lines:
					if "Position" in line:
						prev_pos = pos_from_line(line)
					elif "Chat" in line:
						coords.append(prev_pos)
			elif command['name'] == 'quit':
				for line in reversed(lines):
					if "Position" in line:
						coords.append(pos_from_line(line))
						break
			elif command['name'] == 'found':
				for line in lines:
					if "Position" in line:
						prev_pos = pos_from_line(line)
					elif "Found" in line:
						coords.append(prev_pos)
			elif command['name'] == 'weighted_found':
				words_learned = len([x for x in lines if "Found" in x])
				for line in lines:
					if "Position" in line:
						prev_pos = pos_from_line(line)
					elif "Found" in line:
						if prev_pos == None:
							continue
						for i in range(0, words_learned):
							coords.append(prev_pos)
			else:
				raise Exception('Unrecognized command')

			# skipped = [c for c in pos_lines if c[0] * c[0] + c[1] * c[1] <= threshold]
			# for c in skipped:
			# 	print c

			scale = command['scale']
			coords = map(lambda c: (int(c[0] * scale), int(c[1] * scale)), coords)
			# scale = 5
			# coords = map(lambda c: (int(c[0] * scale), int(c[1] * scale)), coords)
			all_coords.append(coords)

			for (x, y) in coords:
				if x > max_x:
					max_x = x
				if y > max_y:
					max_y = y
				if x < min_x:
					min_x = x
				if y < min_y:
					min_y = y
				# print (x, y)

def main(argv):
	global all_coords
	command = commands[argv[1]]
	out = open(command['out_file'], "w+")

	for subdir, dirs, files in os.walk(rootdir):
		if subdir is not rootdir:
				process_files(command, subdir, files)

	print ((min_x, min_y), (max_x, max_y))

	grid = [[0 for i in xrange(max_x - min_x + 1)] for i in xrange(max_y - min_y + 1)]

	all_coords = [item for sublist in all_coords for item in sublist]
	for (x, y) in all_coords:
		grid[y - min_y][x - min_x] += 1

	if command['probability']:
		total = 0
		for row in range (len(grid)):
			for col in range(len(grid[0])):
				total += grid[row][col]
		prob_pos_grid = map(lambda lst: map(lambda k: float(k) / total, lst), grid)
		if command['name'] == 'prob_pos':
			grid = prob_pos_grid

	out.write("[\n")
	for i in range(0, len(grid) - 1):
		out.write(str(grid[i]) + ",\n")
	out.write(str(grid[len(grid) - 1]) + "\n")
	out.write("]\n")
	out.close()

if __name__ == "__main__":
	main(sys.argv)